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
              changeImagesFun([...e.target.files]);
            }}
            accept="image/png, image/jpeg"
            multiple
          />
        </div>
        <div
          className={`choosen-images-wrapper ${
            blobs.length > 0 ? " bg-[#11111b]" : ""
          }`}
        >
          {blobs.map((b) => (
            <ul>
              <li key={b} className="choosen-image">
                <img src={b} />
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
