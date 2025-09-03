export default class Address {
    constructor(path, element) {
        this.#path = path;
        this.#element = element;
    }
    #path;
    #element;
    
    getPath() {
        return this.#path;
    }
    getElement() {
        return this.#element;
    }
}