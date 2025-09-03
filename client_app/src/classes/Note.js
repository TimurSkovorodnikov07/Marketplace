export default class Note {
  constructor(id, name, description) {
    this.#id = id;
    this.#name = name;
    this.#timeOfCreation = new Date();
    this.#description = description;
  }
  #id;
  #name;
  #timeOfCreation;
  #description;
}
