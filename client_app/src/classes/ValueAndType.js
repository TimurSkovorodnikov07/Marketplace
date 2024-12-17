export default class ValueAndType {
  constructor(value, type) {
    this.#value = value;
    this.#type = type;
  }
  #value;
  #type;
}