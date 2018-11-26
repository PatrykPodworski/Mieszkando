export default class HttpService{
    constructor(){
        this.baseUrl = "http://vps561493.ovh.net:8008/api";
        this.methods = {
            "offers" : "/offers"
        }
    }

    getSerchResults(maxPrice, numberOfRooms) {
        var url = this.getUrl("offers");
        url = this.addParameter(url, "maxPrice", maxPrice);
        url = this.addParameter(url, "numberOfRooms", numberOfRooms);

        console.log(url);

    }

    getUrl(method) {
        return this.baseUrl + this.methods[method];
    }

    addParameter(url, name, value){
        if(url.includes('?')){
            url = `${url}&`;
        } else {
            url = `${url}?`;
        }

        return `${url}${name}=${value}`
    }
}

class HttpParameter{
    constructor(name, value){
        this.name = name;
        this.value = value;
    }
}