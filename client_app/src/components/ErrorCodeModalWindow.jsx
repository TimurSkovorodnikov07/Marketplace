import ModalWindow from "./ModalWindow";

export function ErrorCodeModalWindow({ code, text, isActive }) {
  console.log("code: ", code);
  const [isOpen, setIsOpen] = useState(false);

  return (
    <ModalWindow isOpen={isOpen}>
      <div>{text}</div>
    </ModalWindow>
  );
}
