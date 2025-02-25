import { useNavigate } from "react-router-dom";
import { UpdateAndRemoveButtons } from "./UpdateAndRemoveButtons";
import { ImageComponent } from "./ImageComponent";
import { Stars } from "./Stars";

export function ProductCategoryPartialInfoComponent({
  category,
  isForOwner = false,
  onRemove,
}) {
  const navigate = useNavigate();

  function getBaseInfo() {
    return (
      <>
        <div className="text-start">
          <ImageComponent
            imageId={category.mainImageId}
            linkTo={`/products/${category.id}`}
            imageClass="partial-category-image"
          />
          <div className="give-attention-text">{category.price}$</div>
          <div className="mb-4">{category.name}</div>
          <div>
            <Stars
              className="partial-category-star"
              estimation={category.totalEstimation}
            />
          </div>
        </div>
      </>
    );
  }
  return (
    <>
      {isForOwner === true ? (
        <>
          {getBaseInfo()}
          <div className="product-category-buttons">
            <UpdateAndRemoveButtons
              onUpdate={() => navigate(`/products/update/${category.id}`)}
              onRemove={() => onRemove(category)}
              removedCategoryId={category.id}
            />
          </div>
        </>
      ) : (
        <>{getBaseInfo()}</>
      )}
    </>
  );
}
