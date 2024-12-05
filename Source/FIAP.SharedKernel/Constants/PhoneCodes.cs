using FIAP.SharedKernel.Enumerations;

namespace FIAP.SharedKernel.Constants
{
    public static class PhoneCodes
    {
        public static IDictionary<StatesEnum, HashSet<int>> ValidCodes { get; private set; } = new Dictionary<StatesEnum, HashSet<int>>
        {
            {
                StatesEnum.SP, new HashSet<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19 }
            },
            {
                StatesEnum.RJ, new HashSet<int> { 21, 22, 24 }
            },
            {
                StatesEnum.ES, new HashSet<int> { 27, 28 }
            },
            {
                StatesEnum.MG, new HashSet<int> { 31, 32, 33, 34, 35, 37, 38 }
            },
            {
                StatesEnum.PR, new HashSet<int> { 41, 42, 43, 44, 45, 46 }
            },
            {
                StatesEnum.SC, new HashSet<int> { 47, 48, 49 }
            },
            {
                StatesEnum.RS, new HashSet<int> { 51, 53, 54, 55 }
            },
            {
                StatesEnum.DF, new HashSet<int> { 61 }
            },
            {
                StatesEnum.GO, new HashSet<int> { 62, 64 }
            },
            {
                StatesEnum.MT, new HashSet<int> { 65, 66 }
            },
            {
                StatesEnum.MS, new HashSet<int> { 67 }
            },
            {
                StatesEnum.AC, new HashSet<int> { 68 }
            },
            {
                StatesEnum.RO, new HashSet<int> { 69 }
            },
            {
                StatesEnum.BA, new HashSet<int> { 71, 73, 74, 75, 77 }
            },
            {
                StatesEnum.SE, new HashSet<int> { 79 }
            },
            {
                StatesEnum.PE, new HashSet<int> { 81, 87 }
            },
            {
                StatesEnum.AL, new HashSet<int> { 82 }
            },
            {
                StatesEnum.PB, new HashSet<int> { 83 }
            },
            {
                StatesEnum.RN, new HashSet<int> { 84 }
            },
            {
                StatesEnum.CE, new HashSet<int> { 85, 88 }
            },
            {
                StatesEnum.PI, new HashSet<int> { 86, 89 }
            },
            {
                StatesEnum.PA, new HashSet<int> { 91, 93, 94 }
            },
            {
                StatesEnum.AM, new HashSet<int> { 92, 97 }
            },
            {
                StatesEnum.RR, new HashSet<int> { 95 }
            },
            {
                StatesEnum.AP, new HashSet<int> { 96 }
            },
            {
                StatesEnum.MA, new HashSet<int> { 98, 99 }
            },
        };

        public static bool IsCodeValid(int code)
        {
            return ValidCodes
                   .Values
                   .Any(codes => codes.Contains(code));
        }
    }
}
