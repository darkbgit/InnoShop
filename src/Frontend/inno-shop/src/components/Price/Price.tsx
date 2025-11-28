import { Typography } from "@mui/material";

interface props {
  price: number;
  salePrice: number;
  isOnSale: boolean;
}

interface props {
  price: number;
  salePrice: number;
  isOnSale: boolean;
  isAvailable: boolean;
}

const Price = ({ price, salePrice, isOnSale, isAvailable }: props) => {
  if (!isAvailable) return <Typography>Not available</Typography>;
  if (!isOnSale) return <Typography>{price} $</Typography>;
  else
    return (
      <>
        <Typography sx={{ textDecoration: "line-through" }}>
          {price} $
        </Typography>
        <Typography color="red">{salePrice} $</Typography>
      </>
    );
};

export default Price;
