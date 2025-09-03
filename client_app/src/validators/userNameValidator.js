export default function userNameValidator(name) {
  return name.length > 0 && name.length < 24;
}
