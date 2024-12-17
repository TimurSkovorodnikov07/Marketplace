import { useMemo } from "react";

export function ChooseImagesComponent({
  images,
  changeImagesFun,
  showInvalidText,
  labelText,
}) {
  const blobs = useMemo(() => {
    return images.map((x) => URL.createObjectURL(x));
  }, [images]);

  return (
    <>
      <p>{labelText}</p>
      <input
        id="imageInput"
        type="file"
        onChange={(e) => {
          changeImagesFun([...e.target.files]);
        }}
        accept="image/png, image/jpeg"
        multiple
      />
      <div>
        {blobs.map((b) => (
          <span key={b}>
            <img src={b} className="category-image" />
          </span>
        ))}
      </div>
      {images.length == 0 && showInvalidText === true ? (
        <p className="error-text">{"Choose a images"}</p>
      ) : (
        <></>
      )}
    </>
  );
}
