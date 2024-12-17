export function descriptionValidator(des) {
  return des != undefined && des.length <= 500;
}
export function descriptionWithEmptyValidator(des) {
  return (des != undefined && des.length == 0) || descriptionValidator(des);
}
