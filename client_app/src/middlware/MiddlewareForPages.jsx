import { useEffect } from "react";
import { authService } from "../services/authService";
import { useDispatch, useSelector } from "react-redux";
import { useLocation } from "react-router-dom";

export default function MiddlewareForPages({ children }) {
  const dispath = useDispatch();
  const authData = useSelector(x => x.auth);
  const location = useLocation();

  useEffect(() => {
    console.log(`PathName changed to: ${location.pathname}`);

    const middleware = () => {
      authService(dispath, authData);
    };
    middleware();
  }, [location.pathname]);

  return children;
}
