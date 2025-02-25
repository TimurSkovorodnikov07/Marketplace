import { useState } from "react";
import { updateReview } from "../requests/reviewRequests";
import { Stars } from "./Stars";
import { TextAreaComponent } from "./TextAreaComponent";
import accoutIcon from "../files/images/accountIcon.png";
import { ImageComponent } from "./ImageComponent";
import ModalWindow from "./ModalWindow";
import { ReviewComponent } from "./ReviewComponent";
import { RemoveReviewModalWindow } from "./RemoveReviewModalWindow";

export function MyReview({ review, text, estimation, setText, setEstimation }) {
  const [estimationIsValid, setEstimationIsValid] = useState(true);
  const [textIsValid, setTextIsValid] = useState(true);
  const [isOpen, setIsOpen] = useState(false);

  async function onUpdate() {
    try {
      const response = await updateReview(text, estimation, review.id);
      if (response.status === 200) {
        window.location.reload();
      }
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <>
      <li>
        <ReviewComponent review={review} />
        <div className="text-end flex justify-end">
          <button onClick={() => setIsOpen(true)}>Update</button>
          <RemoveReviewModalWindow id={review.id} />
        </div>
      </li>
      {isOpen == true && (
        <ModalWindow isOpen={isOpen} onClosed={() => setIsOpen(false)}>
          <div className="review-wrapper">
            <div className="review-account-logo">
              <ImageComponent
                imageId={accoutIcon}
                imageClass="account-logo-image"
                linkClass="image-in-link account-image-link"
                getFromServer={false}
              />
            </div>
            <div className="review-textarea-wrapper">
              <TextAreaComponent
                id="reviewCreateTextInput"
                beforeValidationFun={(e) => setText(e.target.value)}
                textareaOtherProps={{
                  placeholder: "Write review...",
                  maxlength: 499,
                }}
                defaultValue={review.text}
              />
            </div>
          </div>
          <div className="stars">
            <Stars
              setEstimation={setEstimation}
              starClassName={"review-star"}
              estimation={estimation}
              defaultValue={review.estimation}
            />
          </div>

          {textIsValid === false && (
            <div className="error-text">
              {"Too much bigger or smaller text"}
            </div>
          )}

          {estimationIsValid === false && (
            <div className="error-text">
              {"Stars must be more 0 and no more 10(or equal 10)"}
            </div>
          )}
          <div className="line-on-bottom"></div>
          <button
            className="text-center items-center"
            onClick={async () => {
              const textIsVal = text.length > 10 && text.length < 500;
              const estIsVal = estimation >= 1 && estimation <= 10;

              if (textIsVal === true && estIsVal === true) await onUpdate();
              setTextIsValid(textIsVal);
              setEstimationIsValid(estIsVal);
            }}
          >
            Update
          </button>
        </ModalWindow>
      )}
    </>
  );
}
