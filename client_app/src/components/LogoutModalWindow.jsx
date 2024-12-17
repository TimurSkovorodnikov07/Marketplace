import { useNavigate } from "react-router-dom";
import ModalWindow from "./ModalWindow";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { logoutType } from "../redux/authReducer";

export function LogoutModalWindow() {
  const [isOpen, setIsOpen] = useState(false);
  const dispath = useDispatch();
  const navigate = useNavigate();

  return (
    <div className="account-info-logout-button-wrapper">
      <button
        className="account-info-logout-button"
        onClick={() => setIsOpen(true)}
      >
        Logout
      </button>

      {isOpen === true && (
        <ModalWindow isOpen={isOpen} onClosed={() => setIsOpen(false)}>
          <p>Are you sure you want to log out?</p>
          <button
            onClick={() => {
              dispath({ type: logoutType });
              navigate("/");
            }}
          >
            YES!
          </button>
        </ModalWindow>
      )}
    </div>
  );
}
