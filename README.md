# HotelBooking

A console program providing hotel booking services. Each hotel service (implementing IHotel interface) may provide 4 operations in custom order: checking price, booking a room in external service (represented by IBookingService interface), making payment in external service (IPaymentService) and sending an email to the specified address. As a result, hotel returns a ReservationResult object (containing result of each operation and generated reservation number). Each hotel is specified by an ID, calling a correct hotel is made by HotelManager.

"Program" project is a simple demonstration (with dummy IBookingService and IPaymentService implementations). All unit tests are in "HotelBookingTests" project.
