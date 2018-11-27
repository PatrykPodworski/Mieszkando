export default class HttpService{
    constructor(){
        this.baseUrl = "http://vps561493.ovh.net:8008/api";
        this.methods = {
            "offers" : "/offers"
        }
    }

    async getSerchResultsAsync(maxCost, numberOfRooms) {
        let url = this.getUrl("offers");
        url = this.addParameter(url, "maxCost", maxCost);
        url = this.addParameter(url, "numberOfRooms", numberOfRooms);

        const response = await fetch(url);
        const offers = await response.json();

        return offers;
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