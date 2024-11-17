#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorAsCode.V1;
using AnimatorAsCode.V1.ModularAvatar;
using AnimatorAsCode.V1.VRC;
using GraphProcessor;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;
using MA_Wrapper = GraphProcessor.ANPUA_ModularAvatar_Wrapper;


[assembly: ExportsPlugin(typeof(ANPUA_AssetGraphProcessor))]
namespace GraphProcessor
{

    public class ANPUA_AssetGraph : MonoBehaviour, IEditorOnly
    {
        public BaseGraph graphData;
    }


    public class ANPUA_AssetGraphProcessor : Plugin<ANPUA_AssetGraphProcessor>
    {

        public override string QualifiedName => "dev.hai-vr.docs.animator-as-code.maac-clothsystem-toggle";
        public override string DisplayName => "Clothsystem Toggle";

        private const string SystemName = "ClothSysMAAC";
        private const bool UseWriteDefaults = true;

        private AacFlLayer baseLayer; protected override void Configure()
        {
            Debug.Log("Configuring " + DisplayName);
            InPhase(BuildPhase.Generating).Run($"Generate {DisplayName}", Generate);
        }


        //Class that can hold a Parameters Name, Type, SyncState and AacFlParameter and MaacParameter
        public class ANPUA_BuildCache_Parameter
        {
            public string name;
            public ANPUA_ParameterType type;
            public ANPUA_ParameterSyncState syncState;
            public ANPUA_ParameterState state;
            public AacFlParameter aacParameter;
            public MaacParameter<int> maacParameterInt;
            public MaacParameter<float> maacParameterFloat;
            public MaacParameter<bool> maacParameterBool;


            public ANPUA_BuildCache_Parameter(string name, ANPUA_ParameterType type, ANPUA_ParameterSyncState syncState, ANPUA_ParameterState state, AacFlParameter aacParameter, MaacParameter<int> maacParameterInt = null, MaacParameter<float> maacParameterFloat = null, MaacParameter<bool> maacParameterBool = null)
            {
                this.name = name;
                this.type = type;
                this.syncState = syncState;
                this.state = state;
                this.aacParameter = aacParameter;

                this.maacParameterInt = maacParameterInt;
                this.maacParameterFloat = maacParameterFloat;
                this.maacParameterBool = maacParameterBool;
            }

            //get the MaacParameter
            public MaacParameter<T> GetMaacParameter<T>()
            {
                if (typeof(T) == typeof(int))
                {
                    return maacParameterInt as MaacParameter<T>;
                }
                else if (typeof(T) == typeof(float))
                {
                    return maacParameterFloat as MaacParameter<T>;
                }
                else if (typeof(T) == typeof(bool))
                {
                    return maacParameterBool as MaacParameter<T>;
                }
                else
                {
                    return null;
                }
            }
        }


        private List<ANPUA_BuildCache_Parameter> parametercache;

        private void Generate(BuildContext ctx)
        {

            Debug.Log("" + ctx.ToString());

            //get the Component and Gameobject for the Settings etc.
            var components = ctx.AvatarRootTransform.GetComponentsInChildren<ANPUA_AssetGraph>(false);
            if (components.Length == 0) return; // If there are none in the avatar, skip this entirely.

            //Get the Graph from the Component
            var graphData = components[0].graphData;
            if (graphData == null) return;


            // Initialize Animator As Code.
            var aac = AacV1.Create(new AacConfiguration
            {
                SystemName = SystemName,
                AnimatorRoot = ctx.AvatarRootTransform,
                DefaultValueRoot = ctx.AvatarRootTransform,
                AssetKey = GUID.Generate().ToString(),
                AssetContainer = ctx.AssetContainer,
                ContainerMode = AacConfiguration.Container.OnlyWhenPersistenceRequired,
                DefaultsProvider = new AacDefaultsProvider(true),
            });
            var ctrl = aac.NewAnimatorController();

            MaAc modularAvatar = MaAc.Create(new GameObject(SystemName) { transform = { parent = ctx.AvatarRootTransform } });
            //MaAc menuTarget = MaAc.Create(new GameObject(SystemName + "maS") { transform = { parent = ctx.AvatarRootTransform } });

            //add Layer
            baseLayer = ctrl.NewLayer("BaseLayer");

            parametercache = GenerateParameters(graphData, baseLayer, modularAvatar);

            //Get the Gameobject of the component as the target for the Set
            var target = components[0].gameObject;
            modularAvatar.NewMergeAnimator(ctrl.AnimatorController, VRCAvatarDescriptor.AnimLayerType.FX);
        }

        //Recursive Function to Generate the Menu, Submenus and MenuItems
        private void GenerateMenu(BaseGraph graph, GameObject target, MaAc modularAvatar, ANPUA_StartNode descriptor)
        {
            //Create the Menu
            GameObject menuObject;
            (menuObject, _) = MA_Wrapper.createSubMenu(modularAvatar, target, descriptor.name, null);

            //get the Connected Nodes using the GetConnectedNodes Function
            var connectedNodes = descriptor.GetPort(nameof(ANPUA_StartNode.link_OUT_Menu), null).GetEdges().Select(e => e.inputNode).ToList();
            foreach (var node in connectedNodes)
            {
                IterateNodes(node, modularAvatar, menuObject);
            } 

            //Iterate all connected Nodes
            // foreach (var node in connectedNodes)
            // {
            //     if (node is ANPUA_Menu)
            //     {
            //         ANPUA_Menu menu = node as ANPUA_Menu;
            //         GenerateMenu(graph, menuObject, modularAvatar, menu);
            //     }
            // }


            // //Create the MenuItems
            // foreach (var node in connectedNodes)
            // {
            //     if (node is ANPUA_MenuItem)
            //     {
            //         ANPUA_MenuItem menuItem = node as ANPUA_MenuItem;
            //         GameObject menuItemObject = createSubMenuWithIconOn(modularAvatar, menuObject, menuItem.name, null);
            //         GenerateMenu(graph, menuItemObject, modularAvatar, menuItem);
            //     }
            // }


        }

        //Function to iterate a Nodes nodes Connected Nodes (Recursive)
        private void IterateNodes(BaseNode node, MaAc modularAvatar, GameObject menuObject)
        {
            // get the link_OUT port
            if (node is ANPUA_Menu)
            {
                ANPUA_Menu menu = node as ANPUA_Menu;
                //menuObject = createSubMenu(modularAvatar, menuObject, menu.name, null);
                IterateNodes(menu.GetPort(nameof(ANPUA_Menu.link_Menu_OUT), null).GetEdges().Select(e => e.inputNode).FirstOrDefault(), modularAvatar, menuObject);
            }
            else if (node is ANPUA_Menu_Toggle)
            {
                ANPUA_Menu_Toggle toggle = node as ANPUA_Menu_Toggle;
                //get the Input port link_IN_Parameter
                var parameter = toggle.GetPort(nameof(ANPUA_Menu_Toggle.link_IN_Parameter), null).GetEdges().Select(e => e.inputNode).FirstOrDefault();
                if (parameter == null) return;
                //get the name of the parameter Node
                var parameterName = parameter.name;
              
                //createToggle(modularAvatar, menuObject, toggle.togglename, FindParameter(parametercache,parameterName).aacParameter as AacFlIntParameter, 0, toggle.icon);
                IterateNodes(toggle.GetPort(nameof(ANPUA_Menu_Toggle.link_Menu_OUT), null).GetEdges().Select(e => e.inputNode).FirstOrDefault(), modularAvatar, menuObject);
            }
            var link_OUT = node.GetPort(nameof(ANPUA_Menu.link_Menu_OUT), null);
        }


        //Find the Descriptor Node in the Graph  in the Nodes and return the first one
        private ANPUA_Parameter_Node FindDescriptorNode(BaseGraph graph)
        {
            return graph.nodes.Find(n => n is ANPUA_Parameter_Node) as ANPUA_Parameter_Node;
        }

        //Function to Generate all Parameters
        private List<ANPUA_BuildCache_Parameter> GenerateParameters(BaseGraph graph, AacFlLayer layer, MaAc modularAvatar)
        {

            //List of AacFlIntParameter
            List<ANPUA_BuildCache_Parameter> parametercache = new List<ANPUA_BuildCache_Parameter>();



            graph.parameters.ForEach(p =>
            {
                if (p.type == ANPUA_ParameterType.Int)
                {

                    AacFlIntParameter param = layer.IntParameter(p.name);
                    MaacParameter<int> menuparam = modularAvatar.NewParameter(param);
                    SetParameterState(menuparam, p.syncState, p.state);
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param, menuparam));
                }
                else if (p.type == ANPUA_ParameterType.Float)
                {
                    AacFlFloatParameter param = layer.FloatParameter(p.name);
                    MaacParameter<float> menuparam = modularAvatar.NewParameter(param);
                    SetParameterState(menuparam, p.syncState, p.state);
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param, null, menuparam));
                }
                else if (p.type == ANPUA_ParameterType.Bool)
                {
                    AacFlBoolParameter param = layer.BoolParameter(p.name);
                    MaacParameter<bool> menuparam = modularAvatar.NewParameter(param);
                    SetParameterState(menuparam, p.syncState, p.state);
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param, null, null, menuparam));
                }

            });

            //Function to set a MaacParameter unsynch and unsaved if conditions are met
            void SetParameterState<T>(MaacParameter<T> param, ANPUA_ParameterSyncState syncState, ANPUA_ParameterState state)
            {
                if (syncState == ANPUA_ParameterSyncState.Unsynch)
                {
                    param.NotSynced();
                }

                if (state == ANPUA_ParameterState.Unsaved)
                {
                    param.NotSaved();
                }
            }
            return parametercache;
        }

        //Function to find a Parameter in the List of Parameters
        private ANPUA_BuildCache_Parameter FindParameter(List<ANPUA_BuildCache_Parameter> parametercache, string name)
        {
            return parametercache.FirstOrDefault(p => p.name == name);
        }









    }
}
#endif
