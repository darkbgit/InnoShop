import { Box } from "@mui/material";
import LoginMenu from "../LoginMenu/LoginMenu";
import { Link } from "react-router";

const Header = () => {
  return (
    <div>
      <Box p={2}>
        <nav style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
          <Link to="/">Home</Link>
          <LoginMenu />
        </nav>
      </Box>
      <hr />
    </div>
  );
};

export default Header;
