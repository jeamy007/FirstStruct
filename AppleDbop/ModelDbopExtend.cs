using System.Data;
using System.Collections.Generic;

namespace AppleDbop
{
    public static class ModelDbopExtend
    {
        public static T GetModel<T>(this DataRow dr) where T : ModelDbopBase
        {
            return ModelDbopBase.GetModelByDataRow<T>(dr);
        }
        //public static T Find<T>(this T model, string wh) where T : ModelDbopBase
        //{
        //    return null;
        //    //return model.Find<T>(model, wh);
        //}


        public static List<T> ListFromDataTable<T>(this DataTable dt) where T : ModelDbopBase
        {
            List<T> rl = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                rl.Add(dr.GetModel<T>());
            }
            return rl;
        }
    }
}
