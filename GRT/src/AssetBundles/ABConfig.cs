using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GRT.AssetBundles
{
    public class ABConfig : ScriptableObject
    {
        public string Platform => Application.platform.ToString();

        public string serverURL = "http://127.0.0.1:8080";
        public int version = 1;
    }
}
