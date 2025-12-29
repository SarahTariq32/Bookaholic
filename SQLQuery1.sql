UPDATE [dbo].[Orders]
SET PaymentMethod = 'N/A'
WHERE PaymentMethod IS NULL;

ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT DF_Orders_PaymentMethod DEFAULT ('N/A') FOR [PaymentMethod];