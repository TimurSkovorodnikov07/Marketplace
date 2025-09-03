import { useState } from "react";
import ModalWindow from "./ModalWindow";
import { categoryRemove } from "../requests/productCategoriesRequests";
import { useResponseCode } from "../hooks/customHooks";
import { useNavigate } from "react-router-dom";

export default function CategoryRemoveModalWindow({
  removableCategoryId,
  removedFun,
}) {
  const [modaWindowIsOpen, setOpenModaWindow] = useState(false);
  const [codeAndText, setCodeAndText] = useResponseCode();
  const navigate = useNavigate();

  async function onRemove() {
    try {
      const response = await categoryRemove(removableCategoryId);
      if (response.status == 200) {
        removedFun?.();
        setOpenModaWindow(false);

        navigate("/");
      }
      setCodeAndText({ code: response.status, text: response.statusText });
    } catch (error) {
      console.error(error);
      setCodeAndText({
        code: error.response.status,
        text: error.response.data,
      });
    }
  }

  return (
    <>
      {modaWindowIsOpen == true && (
        <ModalWindow
          isOpen={modaWindowIsOpen}
          onClosed={() => setOpenModaWindow(false)}
        >
          {codeAndText.code === 403 ? (
            <div>You is not owner</div>
          ) : (
            <>
              <h2>Are you sure you want to delete the product category?</h2>
              <button
                className="modal-button"
                onClick={async () => await onRemove()}
              >
                Yes!
              </button>
            </>
          )}
        </ModalWindow>
      )}
      <button
        className="product-category-remove-button"
        onClick={() => setOpenModaWindow(true)}
      >
        Remove
      </button>
    </>
  );
}
