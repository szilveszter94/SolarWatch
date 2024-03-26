/* eslint-disable react/prop-types */
import { createContext, useState } from "react";

// Create the Snackbar context
export const SnackbarContext = createContext({
  snackbar: {
    open: false,
    message: "",
    type: undefined,
  },
  setSnackbar: () => {},
});

// Define the SnackbarProvider component
export const SnackbarProvider = ({ children }) => {
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: "",
    type: undefined,
  });
  const value = { snackbar, setSnackbar };

  return (
    <SnackbarContext.Provider value={value}>
      {children}
    </SnackbarContext.Provider>
  );
};
