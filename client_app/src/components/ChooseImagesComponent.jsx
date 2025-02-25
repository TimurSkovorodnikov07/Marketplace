import { useMemo } from "react";

export function ChooseImagesComponent({
  images,
  changeImagesFun,
  showInvalidText,
  labelText,
  buttonText,
}) {
  const blobs = useMemo(() => {
    return images.map((x) => URL.createObjectURL(x));
  }, [images]);

  return (
    <>
      <p className="give-attention-text">{labelText}</p>
      <div className="choose-images-wrapper">
        <div className="choose-images-button-wrapper">
          <label className="choose-images-label" htmlFor="imageInput">
            {buttonText}
          </label>
          <input
            className="choose-images-button"
            id="imageInput"
            type="file"
            onChange={(e) => {
              const validFiles = [...e.target.files].filter((file) =>
                ["image/jpg", "image/png", "image/jpeg"].includes(file.type)
              );
              if (validFiles.length > 0) {
                console.log("Valid Files: ", validFiles);
                changeImagesFun(validFiles);
              }
            }}
            accept="image/png, image/jpeg, image/jpg"
            multiple
          />
        </div>
        <div
          className={`choosen-images-wrapper ${
            blobs.length > 0 ? " bg-[#11111b]" : ""
          }`}
        >
          {blobs.map((b, index) => (
            <ul key={index}>
              <li className="choosen-image">
                <img src={b} alt={`Selected ${index}`} />
              </li>
            </ul>
          ))}
        </div>
        {images.length == 0 && showInvalidText === true && (
          <p className="error-text">{"You didn't choose the images"}</p>
        )}
      </div>
    </>
  );
}
