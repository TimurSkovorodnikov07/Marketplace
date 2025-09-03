import { useState } from "react";
import { removeReview } from "../requests/reviewRequests";
import ModalWindow from "./ModalWindow";

export function RemoveReviewModalWindow({ id }) {
  const [isOpen, setIsOpen] = useState(false);

  async function onRemove() {
    try {
      const response = await removeReview(id);

      if (response.status === 200) {
        window.location.reload();
      }
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <>
      <div>
        <button onClick={() => setIsOpen(true)}>Remove</button>
      </div>
      {isOpen === true && (
        <ModalWindow isOpen={isOpen} onClosed={() => setIsOpen(false)}>
          <div>Are you sure you deleted this review?</div>
          <button onClick={async () => await onRemove()}>YES!</button>
        </ModalWindow>
      )}
    </>
  );
}
