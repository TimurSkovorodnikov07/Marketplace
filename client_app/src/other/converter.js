export function stringToBool(str) {
  if (str == "true" || str == "True") return true;
  else if (str == "false" || str == "False") return false;
  return undefined;
}
