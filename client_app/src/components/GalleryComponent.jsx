import { useMemo, useState } from "react";
import { getPathToImage } from "./ImageComponent";

export function GalleryComponent({ imagesIdentifiers = [] }) {
  const list = useMemo(() => {
    const resultArray = [];
    let curIndex = 0;

    while (curIndex < imagesIdentifiers.length) {
      resultArray.push({ index: curIndex, value: imagesIdentifiers[curIndex] });
      curIndex++;
    }
    return resultArray;
  }, [imagesIdentifiers]);

  const [selectedImageAndIndex, setSelectedImageAndIndex] = useState({
    index: 0,
    value: imagesIdentifiers[0],
  });

  function move(isIncrepent) {
    const curIndex = selectedImageAndIndex.index;
    const newSelectedIndex = isIncrepent === true ? curIndex + 1 : curIndex - 1;
    setSelectedImageAndIndex({
      index: newSelectedIndex,
      value: imagesIdentifiers[newSelectedIndex],
    });
  }

  return (
    <>
      {list.length > 1 ? (
        <>
          <div className="gallery-small-images-wrapper">
            {list.map((x) => (
              <div key={x.value}>
                <button
                  className={"gallery-change-button"}
                  onClick={() => setSelectedImageAndIndex(x)}
                >
                  <img
                    src={getPathToImage(x.value)}
                    className={
                      x.index === selectedImageAndIndex.index
                        ? "gallery-selected-small-image"
                        : "gallery-small-image"
                    }
                  />
                </button>
              </div>
            ))}
          </div>
          <div className={"gallery-image-wrapper"}>
            <button
              onClick={() => move(false)}
              className={
                selectedImageAndIndex.index > 0
                  ? "gallery-prev-image-button"
                  : "gallery-prev-image-button-negative"
              }
            >
              {"<-"}
            </button>

            <img
              src={getPathToImage(selectedImageAndIndex.value)}
              className={"gallery-image"}
            />

            <button
              onClick={() => move(true)}
              className={
                selectedImageAndIndex.index < list.length - 1
                  ? "gallery-next-image-button"
                  : "gallery-next-image-button-negative"
              }
            >
              {"->"}
            </button>
          </div>
        </>
      ) : (
        <div className={"gallery-image-wrapper"}>
          <img
            src={getPathToImage(selectedImageAndIndex.value)}
            className={"gallery-image"}
          />
        </div>
      )}
    </>
  );
}
