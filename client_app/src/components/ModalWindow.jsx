import "../styles/modalWindow.css";
import { useState } from "react";

export default function ModalWindow({ isOpen, onClosed, children }) {
  const [isExiting, setIsExiting] = useState(false);

  const handleClose = () => {
    setIsExiting(true); // Start exit animation
    setTimeout(() => {
      setIsExiting(false); // Reset exit state
      onClosed(); // Trigger the parent's close handler
    }, 300); // Match duration of exit animation
  };

  return (
    isOpen && (
      <div
        className={`modal-wrapper ${isExiting ? "fade-out" : ""}`}
        onClick={handleClose}
      >
        <div
          className={`modal-content-wrapper ${
            isExiting ? "slide-out" : "slide-in"
          }`}
          onClick={(e) => e.stopPropagation()} // Prevent closing when clicking inside the modal
        >
          <div className="modal-content">{children}</div>
          <button
            className="modal-close-button"
            onClick={(e) => {
              e.stopPropagation();
              handleClose();
            }}
          >
            x
          </button>
        </div>
      </div>
    )
  );
}