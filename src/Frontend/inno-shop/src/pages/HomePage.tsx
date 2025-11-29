import { Box, Typography, Container, Paper } from "@mui/material";

const HomePage = () => {
  return (
    <Container maxWidth="md">
      <Box sx={{ my: 4, textAlign: "center" }}>
        <Paper elevation={3} sx={{ p: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Welcome to InnoShop!
          </Typography>
          <Typography variant="body1" gutterBottom>
            This is a simple application where you can browse products.
          </Typography>
          <Typography variant="body1" gutterBottom>
            If you log in as a regular user, you can make products and edit or
            delete your products.
          </Typography>
          <Typography variant="body1">
            If you log in as an admin, you can delete users.
          </Typography>
        </Paper>
      </Box>
    </Container>
  );
};

export default HomePage;
