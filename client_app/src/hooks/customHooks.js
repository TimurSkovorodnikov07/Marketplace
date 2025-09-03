import { useState } from "react";

export function useResponseCode() {
  return useState({
    code: 0,
    text: "",
  });
}
