import { Box, Typography } from "@mui/material";

const HomePage = () => {
  return (
    <Box alignContent={"center"}>
      <Typography>Welcome to InnoShop!</Typography>
      <p>When unauthorized you can view all products.</p>
      <p>
        When log in as user you can create new product and edit products created
        by user.
      </p>
      <p>When log in as admin you can delete users.</p>
    </Box>
  );
};

export default HomePage;
