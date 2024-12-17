import { imagesSource } from "../configs/sources";
import { Link } from "react-router-dom";

export function getPathToImage(id, getFromServer = true) {
  return getFromServer === true ? `${imagesSource}/${id}` : id;
}

export function ImageComponent({
  imageId,
  linkTo,
  imageClass,
  linkClass,
  getFromServer = true,
}) {
  function getImage() {
    return getPathToImage(imageId, getFromServer);
  }

  return linkTo === undefined ? (
    <img src={getImage()} className={imageClass} />
  ) : (
    <Link className={linkClass} to={linkTo}>
      <img src={getImage()} className={imageClass} />
    </Link>
  );
}
