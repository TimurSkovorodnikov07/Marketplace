import CategoryRemoveModalWindow from "./CategoryRemoveModalWindow";

export function UpdateAndRemoveButtons({
  onUpdate,
  onRemove,
  removedCategoryId,
}) {
  return (
    <>
      <button
        className="product-category-update-button"
        onClick={() => onUpdate()}
      >
        Update
      </button>
      <CategoryRemoveModalWindow
        removableCategoryId={removedCategoryId}
        removedFun={() => onRemove}
      />
    </>
  );
}
