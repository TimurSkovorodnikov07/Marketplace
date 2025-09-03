export default function categoryNameValidator(name) {
  return name != undefined && name.length > 0 && name.length < 24;
}
