import type { Product } from "../../interfaces/product.interface";
import Price from "../Price/Price";
import { Box, Card, CardContent, Grid, Typography } from "@mui/material";

const ProductDetailCard = ({ product }: { product: Product }) => {
  return (
    <Card>
      <Grid container>
        <Grid>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "space-between",
              height: "100%",
            }}
          >
            <CardContent>
              <Typography variant="body2" color="text.secondary">
                Category: {product.categoryName}
              </Typography>
              <Typography gutterBottom variant="h5" component="div">
                {product.name}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {product.description}
              </Typography>
              <Price
                price={product.price}
                isOnSale={product.isOnSale}
                salePrice={product.salePrice}
                isAvailable={product.isAvailable}
              />
            </CardContent>
          </Box>
        </Grid>
      </Grid>
    </Card>
  );
};

export default ProductDetailCard;
