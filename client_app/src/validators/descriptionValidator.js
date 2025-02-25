export function descriptionValidator(des) {
  return des != undefined && des.length <= 500;
}
export function descriptionValidatorForSeller(des) {
  return des != undefined && des.length <= 125;
}
export function descriptionWithEmptyValidator(des) {
  return (des != undefined && des.length == 0) || descriptionValidator(des);
}
