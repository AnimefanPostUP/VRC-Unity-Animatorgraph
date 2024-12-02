using UnityEngine;
namespace GraphProcessor.Builder
{
    [System.Serializable]
    public class AnimatorCondition
    {
        // Enum for comparison, e.g. Greater, Less, Equal, GreaterOrEqual, LessOrEqual, NotEqual
        [SerializeField]
        public CompareEnum compareMode = CompareEnum.Greater;

        [SerializeField]
        public ANPUA_GenericParameter parameter;

        [SerializeField]
        public string parameterName = "";


        // Backing fields with default values
        [SerializeField]
        private bool _boolValue = false;

        [SerializeField]
        private int _intValue = 0;

        [SerializeField]
        private int _intValue_B = 0;

        [SerializeField]
        private float _floatValue = 0f;

        [SerializeField]
        private float _floatValue_B = 0f;

        // Setters that set a bool, int, and float if one of them is set
        // The bool is false on int 0 and float below 0.5
        // The int will be 1 or 0 based on true or false of the bool or when a float is set, then it's rounded to the nearest int
        // The float will be 0 or 1 based on the bool or if an int is set, it's just copied
        public bool boolValue
        {
            get => _boolValue;
            set
            {
                _boolValue = value;
                _intValue = value ? 1 : 0;
                _floatValue = value ? 1 : 0;
            }
        }

        public int intValue
        {
            get => _intValue;
            set
            {
                _intValue = value;
                _boolValue = value != 0;
                _floatValue = value;
            }
        }

        // Getters and setters for the int value B, only Affects the int B and float B, NOT the bool or the first int and float
        public int intValue_B
        {
            get => _intValue_B;
            set
            {
                _intValue_B = value;
                _floatValue_B = value;
            }
        }



        public float floatValue
        {
            get => _floatValue;
            set
            {
                _floatValue = value;
                _boolValue = value > 0.5f;
                _intValue = Mathf.RoundToInt(value);
            }
        }

                // Getters and setters for the float value B, only Affects the float B and int B, NOT the bool or the first int and float
        public float floatValue_B
        {
            get => _floatValue_B;
            set
            {
                _floatValue_B = value;
                _intValue_B = Mathf.RoundToInt(value);
            }
        }
        

        // Get Value that returns "none" if the parameter is null or a value based on the parameter's type
        public object GetValue()
        {
            if (parameter == null)
            {
                return floatValue;
            }
            switch (parameter.type)
            {
                case ANPUA_ParameterType.Bool:
                    return boolValue;
                case ANPUA_ParameterType.Int:
                    return intValue;
                case ANPUA_ParameterType.Float:
                    return floatValue;
                default:
                    return "none";
            }
        }

        //Get Value B
        public object GetValue_B()
        {
            if (parameter == null)
            {
                return floatValue_B;
            }
            switch (parameter.type)
            {
                case ANPUA_ParameterType.Int:
                    return intValue_B;
                case ANPUA_ParameterType.Float:
                    return floatValue_B;
                default:
                    return "none";
            }
        }

        //Set Value
        public void SetValue(object value)
        {


            if (parameter == null)
            {
                floatValue = (float)value;
            }
            switch (parameter.type)
            {
                case ANPUA_ParameterType.Bool:
                    boolValue = (bool)value;
                    break;
                case ANPUA_ParameterType.Int:
                    intValue = (int)value;
                    break;
                case ANPUA_ParameterType.Float:
                    floatValue = (float)value;
                    break;
            }
        }

        //Set Value B
        public void SetValue_B(object value)
        {
            if (parameter == null)
            {
                floatValue_B = (float)value;
            }
            switch (parameter.type)
            {
                case ANPUA_ParameterType.Int:
                    intValue_B = (int)value;
                    break;
                case ANPUA_ParameterType.Float:
                    floatValue_B = (float)value;
                    break;
            }
        }

        //Set Parameter
        public void SetParameter(ANPUA_GenericParameter parameter)
        {
            if (parameter == null) return;
            this.parameter = parameter;
            parameterName = parameter.name;
        }

        public string GetParameterName()
        {
            return parameter != null ? parameter.name : parameterName;
        }
    }

    public enum CompareEnum
    {
        Greater,
        Less,
        Equal,
        GreaterOrEqual,
        LessOrEqual,
        NotEqual,
        Inside,
        Outside
    }
}