import {
  Box,
  Typography,
  Container,
  Paper,
  Grid,
  Divider,
} from "@mui/material";

const HomePage = () => {
  return (
    <Box>
      <Box
        sx={{
          bgcolor: "primary.main",
          color: "primary.contrastText",
          py: 10,
          textAlign: "center",
        }}
      >
        <Container maxWidth="md">
          <Typography
            variant="h2"
            component="h1"
            fontWeight="bold"
            gutterBottom
          >
            Welcome to InnoShop!
          </Typography>
        </Container>
      </Box>

      <Box sx={{ py: 6, bgcolor: "background.default" }}>
        <Container maxWidth="lg">
          <Typography
            variant="h4"
            textAlign="center"
            gutterBottom
            color="text.secondary"
            sx={{ mb: 4 }}
          >
            Demo Accounts
          </Typography>
          <Grid container spacing={4} justifyContent="center">
            <Grid>
              <Paper elevation={3} sx={{ p: 3, textAlign: "center" }}>
                <Typography variant="h5" component="h3" gutterBottom>
                  User Access
                </Typography>
                <Typography
                  variant="body1"
                  color="text.secondary"
                  sx={{ mb: 2 }}
                >
                  Log in as a user to create, edit, or delete your own products.
                </Typography>
                <Divider sx={{ my: 2 }} />
                <Typography variant="body2">
                  <strong>user1@user.com</strong> | UserUser1!
                </Typography>
                <Typography variant="body2">
                  <strong>user2@user.com</strong> | UserUser2!
                </Typography>
                <Typography variant="body2">
                  <strong>user3@user.com</strong> | UserUser3!
                </Typography>
              </Paper>
            </Grid>

            <Grid>
              <Paper elevation={3} sx={{ p: 3, textAlign: "center" }}>
                <Typography variant="h5" component="h3" gutterBottom>
                  Admin Access
                </Typography>
                <Typography
                  variant="body1"
                  color="text.secondary"
                  sx={{ mb: 2 }}
                >
                  Log in as an admin to manage users across the platform.
                </Typography>
                <Divider sx={{ my: 2 }} />
                <Typography variant="body2">
                  <strong>admin@admin.com</strong> | AdminAdmin1!
                </Typography>
              </Paper>
            </Grid>
          </Grid>
        </Container>
      </Box>
    </Box>
  );
};

export default HomePage;
