export default class HttpService{
    constructor(){
        this.baseUrl = "http://mieszkando.pl:8008/api";
        this.methods = {
            "simpleSearch" : "/offers",
            "advancedSearch": "/offers/advanced"
        }
    }

    async getSerchResultsAsync(maxCost, numberOfRooms) {
        let url = this.getUrl("simpleSearch");
        url = this.addParameter(url, "maxCost", maxCost);
        url = this.addParameter(url, "numberOfRooms", numberOfRooms);

        const response = await fetch(url)
            .then(this.handleErrors);

        const offers = await response.json();

        return offers;
    }

    async getAdvancedSearchResultsAsync(criteria) {
        const url = this.getUrl("advancedSearch");
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(criteria)
        })
        .then(this.handleErrors);

        const result = await response.json();

        return result;
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

    handleErrors(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response;
    }
}