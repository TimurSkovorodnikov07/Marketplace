import "./styles/index.css";
import "./styles/input.css";

import AppRouting from "./components/AppRouting";
import { useSelector } from "react-redux";
import { Topbar } from "./components/Topbar";
import { FooterComponent } from "./components/FooterComponent";

function App() {
  const auth = useSelector((state) => state.auth);
  return (
    <div className="app">
      <header className="topbar">
        <Topbar auth={auth} />
      </header>

      <section className="app-body">
        <div className="content-wrapper">
          <AppRouting isAuth={auth.isAuth} />
        </div>
      </section>
      <FooterComponent />
    </div>
  );
}

export default App;
