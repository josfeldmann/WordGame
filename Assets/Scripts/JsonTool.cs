public static class JsonTool {


    public static string ObjectToString<T>(T t) {
        // return JsonUtility.ToJson(t);
        return Newtonsoft.Json.JsonConvert.SerializeObject(t);
    }

    public static T StringToObject<T>(string json) {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }

}

