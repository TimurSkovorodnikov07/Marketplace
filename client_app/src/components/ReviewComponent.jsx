import { ImageComponent } from "./ImageComponent";
import { Stars } from "./Stars";
import accoutIcon from "../files/images/accountIcon.png";

export function ReviewComponent({ review }) {
  return (
    <>
      <div className="flex items-center">
        <div className="review-account-logo">
          <ImageComponent
            imageId={accoutIcon}
            imageClass="account-logo-image"
            linkClass="image-in-link account-image-link"
            getFromServer={false}
          />
        </div>
        <p className="break-all">{review.text}</p>
      </div>
      <Stars estimation={review.estimation} />
    </>
  );
}
