import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { goToLogin } from "../services/navigateService";

export function AuthenticationFiter({ children }) {
  const navigate = useNavigate();
  const isAuth = useSelector((state) => state.auth.isAuth);

  useEffect(() => {
    if (isAuth !== true) goToLogin(navigate);
  }, [isAuth]);

  return <>{children}</>;
}
