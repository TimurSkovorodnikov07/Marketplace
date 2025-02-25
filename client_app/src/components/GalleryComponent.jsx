import { useEffect, useMemo, useState } from "react";
import { getImagesByCategoryId } from "../requests/imagesRequest";

export function GalleryComponent({ categoryId }) {
  const [images, setImages] = useState([]);
  const [selectedImage, setSelectedImage] = useState(null);

  useEffect(() => {
    const fetchImages = async () => {
      try {
        const response = await getImagesByCategoryId(categoryId);

        if (response.status === 200) {
          setImages(response.data);
          setSelectedImage({ data: response.data[0], index: 0 });
        }
      } catch (error) {
        console.error("Error fetching images:", error);
      }
    };
    fetchImages();
  }, [categoryId]);

  const list = useMemo(
    () => images.map((img, index) => ({ index, ...img })), // Keep full image object
    [images]
  );

  function move(isIncrement) {
    if (!selectedImage) return;

    const newIndex = isIncrement
      ? selectedImage.index + 1
      : selectedImage.index - 1;

    if (newIndex >= 0 && newIndex < list.length) {
      setSelectedImage({ data: list[newIndex], index: newIndex }); // Select full image object
    }
  }

  function getImageSrc(image) {
    if (!image) return "";
    return `data:${image.mimeType};base64,${image.imageData}`;
  }

  return (
    <>
      {list.length > 1 ? (
        <>
          <div className="gallery-small-images-wrapper">
            {list.map((x, index) => (
              <div key={x.id}>
                <button
                  className="gallery-change-button"
                  onClick={() => setSelectedImage({ data: x, index: index })}
                >
                  <img
                    src={getImageSrc(x)}
                    className={
                      x.index === selectedImage?.index
                        ? "gallery-selected-small-image"
                        : "gallery-small-image"
                    }
                  />
                </button>
              </div>
            ))}
          </div>
          <div className="gallery-image-wrapper">
            <button
              onClick={() => move(false)}
              className={
                selectedImage?.index > 0
                  ? "gallery-prev-image-button"
                  : "gallery-prev-image-button-negative"
              }
            >
              {"←"}
            </button>

            {selectedImage && (
              <img
                src={getImageSrc(selectedImage.data)}
                className="gallery-image"
              />
            )}

            <button
              onClick={() => move(true)}
              className={
                selectedImage?.index < list.length - 1
                  ? "gallery-next-image-button"
                  : "gallery-next-image-button-negative"
              }
            >
              {"→"}
            </button>
          </div>
        </>
      ) : (
        <div className="gallery-image-wrapper">
          {selectedImage && (
            <img
              src={getImageSrc(selectedImage.data)}
              className="gallery-image"
            />
          )}
        </div>
      )}
    </>
  );
}
