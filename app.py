from flask import Flask, request, json
from machine_learning import DataSet
import time, datetime
from multiprocessing.pool import ThreadPool

app = Flask(__name__)
dataSet = DataSet()
dataSet.open()#data = json.loads(request.data)

@app.route("/patent", methods = ["GET"])
def get_patent():
    import requests
    data = request.args
    year = datetime.datetime.now().year
    response = requests.get("https://yandex.ru/patents/api/search",params={
        "text":"",
        "template":f"%request% << (s_19_country:RU | s_19_country:SU) << s_22_date:>={year-1}0101 << s_22_date:<={year}0101 <<  (i_doc_type:1 | i_doc_type:2) && (z_en_73_owner:({data['name']}) | z_ru_73_owner:({data['name']}))",
        "p":0,
        "how":"rlv",
        "numdoc":10}).json()

    return {"response":response["TotalDocCount"][0]}


def get_predict_async(dataSet, params):
    dataSet.open()
    value = float(dataSet.get_predict(params)[0])
    return value


@app.route("/predict", methods = ["GET"])
def get_predict():
    data = request.args
    keys = ["patent","reg","events", "platform", "nalog"]

    for key in keys:
        if key not in data:
            error ={
                "response":[
                    {
                        "error": f"Вы забыли указать параметр \"{key}\""
                        }
                ]
            }
            return str(error)


    _data = {}
    for key in keys:
        _data[key] = float(data[key])

    params = [_data["patent"],_data["reg"],_data["events"],_data["platform"], _data["nalog"]]
    value = []
    pool = ThreadPool(processes=1)
    for i in range(5):
        async_result  = pool.apply_async(get_predict_async, (dataSet, params))

        return_val = async_result.get() 
        value.append(return_val)

    values = {}
    for i in value:
        if str(i) not in values:
            values[str(i)] = value.count(i)
            
    
    list_d = list(values.items())
    list_d.sort(key=lambda i: i[1])
    min_res  = min(list_d)[0]
    response = {
        "response":[
            {
                "values":params,
                "keys":keys,
                "items":_data,
                "result": min_res
            }
        ]
    }
    return response
    