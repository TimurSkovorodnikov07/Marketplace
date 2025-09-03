export default function objectToFormMapper(object) {
  const formData = new FormData();

  for (let key in object) {
    const value = object[key];

    if (value == undefined || value == "" || value == null) continue;

    if (Array.isArray(value)) {
      for (let v of value) {
        formData.append(key, v);
      }
    } else {
      formData.append(key, value);
    }
  }

  return formData;
}
