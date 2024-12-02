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
using MA_Wrapper = GraphProcessor.MaWrapper;


[assembly: ExportsPlugin(typeof(ANPUA_AssetGraphProcessor))]
namespace GraphProcessor
{

    public class ANPUA_AssetGraph : MonoBehaviour, IEditorOnly
    {
        public BaseGraph graphData;
        public GameObject assetTarget;
    }


    public class ANPUA_AssetGraphProcessor : Plugin<ANPUA_AssetGraphProcessor>
    {

        public override string QualifiedName => "dev.hai-vr.docs.animator-as-code.maac-AnimatorGraph";
        public override string DisplayName => "AnimatorGraph";

        private const string SystemName = "AnimatorGraphMAAC";
        private const bool UseWriteDefaults = true;

        private AacFlLayer baseLayer; protected override void Configure()
        {
            Debug.Log("Configuring " + DisplayName);
            InPhase(BuildPhase.Generating).Run($"Generate {DisplayName}", Generate);
        }


     private ANPUA_ParameterManager paramtermanager;

        private void Generate(BuildContext ctx)
        {

            Debug.Log("" + ctx.ToString());

            //get the Component and Gameobject for the Settings etc.
            var components = ctx.AvatarRootTransform.GetComponentsInChildren<ANPUA_AssetGraph>(false);
            if (components.Length == 0) return; // If there are none in the avatar, skip this entirely.

            //Get the Graph from the Component
            var graphData = components[0].graphData;
            if (graphData == null) return;

            var assetTarget = components[0].assetTarget;
            if (assetTarget == null) return;




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


            //Modular AV for the Sets itself
            // MaAc modularAvatar = MaAc.Create(new GameObject(SystemName) { transform = { parent = ctx.AvatarRootTransform } });
            //Modular AV for the Set Menus
            MaAc menuTarget = MaAc.Create(new GameObject(SystemName + "maS") { transform = { parent = ctx.AvatarRootTransform } });

            //MaAc menuTarget = MaAc.Create(new GameObject(SystemName + "maS") { transform = { parent = ctx.AvatarRootTransform } });
            //var ob2 = MA_Wrapper.createSubMenu(menuTarget, assetTarget, "CoreMenu");
            //Add empty toggle with new parameter

            //add Layer
            baseLayer = ctrl.NewLayer("BaseLayer");

            paramtermanager = new ANPUA_ParameterManager(GenerateParameters(graphData, baseLayer, menuTarget));

            //Prepare Menu Container Object
            BuildNode descriptor = FindDescriptorNode(graphData);
            descriptor.menuContainer = new MaItemContainer(menuTarget, assetTarget, null);
            //descriptor.InitializeMenuContainer(modularAvatar, rootObject);

            GenerateMenu(graphData, assetTarget, menuTarget, descriptor);

            //modularAvatar.NewMergeAnimator(ctrl.AnimatorController, VRCAvatarDescriptor.AnimLayerType.FX);
        }

        //Recursive Function to Generate the Menu, Submenus and MenuItems
        private void GenerateMenu(BaseGraph graph, GameObject target, MaAc modularAvatar, BuildNode descriptor)
        {
            //Check all for null
            if (descriptor == null) Debug.LogError("Descriptor is null");
            if (target == null) Debug.LogError("Target is null");
            if (modularAvatar == null) Debug.LogError("ModularAvatar is null");
            if (descriptor == null) Debug.LogError("Descriptor is null");

            //get the Connected Nodes using the GetConnectedNodes Function
            var connectedNodes = descriptor.GetOutputNodes();
            foreach (var node in connectedNodes)
            {
                if (node is MenuBaseNode)
                {
                    MenuBaseNode menuNode = node as MenuBaseNode;
                    EvaluateMenuNode(menuNode, descriptor.menuContainer);
                }
            }
        }

   
        private void EvaluateMenuNode(MenuBaseNode menuNode, MaItemContainer menuContainer)
        {
            if (menuNode == null) return;

            menuNode.ProcessOnBuild();
            menuContainer = menuNode.ProcessMenuOnBuild(menuContainer, paramtermanager);

            var connectedNodes = menuNode.GetOutputNodes();
            foreach (MenuBaseNode node in connectedNodes)
            {
                EvaluateMenuNode(node, menuContainer);
                //Debug the Containers object name
                Debug.Log("Container Object Name: " + menuContainer.menuObject.name);
            }
        }

        /* private void IterateNodes(BaseNode node, MaAc modularAvatar, GameObject menuObject)
                {
                    // get the link_OUT port
                    if (node is MenuNode)
                    {
                        MenuNode menu = node as MenuNode;
                        //menuObject = createSubMenu(modularAvatar, menuObject, menu.name, null);
                        IterateNodes(menu.GetPort(nameof(MenuNode.link_Menu_OUT), null).GetEdges().Select(e => e.inputNode).FirstOrDefault(), modularAvatar, menuObject);
                    }
                    else if (node is MenuNode_Toggle)
                    {
                        MenuNode_Toggle toggle = node as MenuNode_Toggle;
                        //get the Input port link_IN_Parameter
                        var parameter = toggle.GetPort(nameof(MenuNode_Toggle.link_IN_Parameter), null).GetEdges().Select(e => e.inputNode).FirstOrDefault();
                        if (parameter == null) return;
                        //get the name of the parameter Node
                        var parameterName = parameter.name;

                        //createToggle(modularAvatar, menuObject, toggle.togglename, FindParameter(parametercache,parameterName).aacParameter as AacFlIntParameter, 0, toggle.icon);
                        IterateNodes(toggle.GetPort(nameof(MenuNode_Toggle.link_Menu_OUT), null).GetEdges().Select(e => e.inputNode).FirstOrDefault(), modularAvatar, menuObject);
                    }
                    var link_OUT = node.GetPort(nameof(MenuNode.link_Menu_OUT), null);
                }

        */

        //Find the Descriptor Node in the Graph  in the Nodes and return the first one
        private BuildNode FindDescriptorNode(BaseGraph graph)
        {
            return graph.nodes.Find(n => n is BuildNode) as BuildNode;
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
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param,p.IntValue,p.FloatValue, p.BoolValue, menuparam));
                }
                else if (p.type == ANPUA_ParameterType.Float)
                {
                    AacFlFloatParameter param = layer.FloatParameter(p.name);
                    MaacParameter<float> menuparam = modularAvatar.NewParameter(param);
                    SetParameterState(menuparam, p.syncState, p.state);
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param,p.IntValue,p.FloatValue, p.BoolValue, null, menuparam));
                }
                else if (p.type == ANPUA_ParameterType.Bool)
                {
                    AacFlBoolParameter param = layer.BoolParameter(p.name);
                    MaacParameter<bool> menuparam = modularAvatar.NewParameter(param);
                    SetParameterState(menuparam, p.syncState, p.state);
                    parametercache.Add(new ANPUA_BuildCache_Parameter(p.name, p.type, p.syncState, p.state, param,p.IntValue,p.FloatValue, p.BoolValue, null, null, menuparam));
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



    }

    //Create a class to manage and access  ANPUA_BuildCache_Parameter including the FindParameter Function
    public class ANPUA_ParameterManager
    {
        private List<ANPUA_BuildCache_Parameter> parametercache;

        public ANPUA_ParameterManager(List<ANPUA_BuildCache_Parameter> parametercache)
        {
            this.parametercache = parametercache;
        }

        public ANPUA_BuildCache_Parameter FindParameter(string name)
        {
            return parametercache.FirstOrDefault(p => p.name == name);
        }
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

        public int intValue=0;
        public float floatValue=0;
        public bool boolValue=false;


        public ANPUA_BuildCache_Parameter(string name, ANPUA_ParameterType type, ANPUA_ParameterSyncState syncState, ANPUA_ParameterState state, AacFlParameter aacParameter, int intval, float floatval, bool boolval, MaacParameter<int> maacParameterInt = null, MaacParameter<float> maacParameterFloat = null, MaacParameter<bool> maacParameterBool = null)
        {
            this.name = name;
            this.type = type;
            this.syncState = syncState;
            this.state = state;
            this.aacParameter = aacParameter;

            this.intValue = intval;
            this.floatValue = floatval;
            this.boolValue = boolval;

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

}
#endif
