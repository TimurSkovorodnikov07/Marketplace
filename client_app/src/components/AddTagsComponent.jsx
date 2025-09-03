import { InputComponent } from "./InputComponent";
import notNullOrEmptyValidator from "../validators/notNullOrEmptyValidator";
import { useRef } from "react";

export function AddTagsComponent({
  tags,
  addTagFun,
  deleteTagFun,
  labelText,
  showInvalidText,
  onChanged,
}) {
  const newTag = useRef(null);
  return (
    <>
      <p className="give-attention-text">{labelText}</p>
      <div className="add-tag-wrapper">
        <InputComponent
          id="addTag"
          type="text"
          ref={newTag}
          inputOtherProps={{ placeholder: "tag..." }}
        />
        <span>
          <button
            onClick={(e) => {
              const newTagValue = newTag.current.value;
              const existingTag = tags.find((x) => x == newTagValue);

              if (notNullOrEmptyValidator(newTagValue) && !existingTag) {
                addTagFun(newTagValue);
                onChanged?.(e);
              }
            }}
          >
            {"add"}
          </button>
        </span>
      </div>

      <div className="added-tags-wrapper">
        {Array.isArray(tags) &&
          tags.length > 0 &&
          tags.map((x) => (
            <div key={x} >
              <button
                className="added-tag-button"
                onClick={(e) => {
                  onChanged?.(e);
                  deleteTagFun(x);
                }}
              >
                <span className="tag">{x}</span>
              </button>
            </div>
          ))}
      </div>
      {tags.length == 0 &&
      (showInvalidText == undefined || showInvalidText === true) ? (
        <p className="error-text">{"Choose a tags"}</p>
      ) : (
        <></>
      )}
    </>
  );
}
