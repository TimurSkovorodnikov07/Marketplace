import { useState } from "react";
import { Stars } from "./Stars";
import { TextAreaComponent } from "./TextAreaComponent";
import accoutIcon from "../files/images/accountIcon.png";
import { addReview } from "../requests/reviewRequests";
import { ImageComponent } from "./ImageComponent";

export function ReviewWrite({
  categoryId,
  text,
  estimation,
  setText,
  setEstimation,
}) {
  const [estimationIsValid, setEstimationIsValid] = useState(true);
  const [textIsValid, setTextIsValid] = useState(true);

  async function create() {
    try {
      const response = await addReview(text, estimation, categoryId);
      if (response.status === 200) {
        window.location.reload();
      }
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <>
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
              maxLength: 499,
            }}
          />
        </div>
        <button
          onClick={async () => {
            const textIsVal = text.length > 10 && text.length < 500;
            const estIsVal = estimation >= 1 && estimation <= 10;

            if (textIsVal === true && estIsVal === true) await create();
            setTextIsValid(textIsVal);
            setEstimationIsValid(estIsVal);
          }}
        >
          Write
        </button>
      </div>
      <div className="stars">
        <Stars
          setEstimation={setEstimation}
          starClassName={"review-star"}
          estimation={estimation}
        />
      </div>

      {textIsValid === false && (
        <div className="error-text">{"Too much bigger or smaller text"}</div>
      )}

      {estimationIsValid === false && (
        <div className="error-text">
          {"Stars must be more 0 and no more 10(or equal 10)"}
        </div>
      )}
    </>
  );
}
