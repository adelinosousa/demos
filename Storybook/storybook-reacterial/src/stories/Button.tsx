//import "./button.css";
import { Button as ButtonUI } from "@mui/material";

interface ButtonProps {
  backgroundColor?: string;
  size?: "small" | "medium" | "large";
  label: string;
  onClick?: () => void;
}

export const Button = ({
  size = "medium",
  backgroundColor,
  label,
  ...props
}: ButtonProps) => {
  return (
    <ButtonUI
      sx={{ backgroundColor }}
      size={size}
      variant="contained"
      {...props}
    >
      {label}
    </ButtonUI>
  );
};
