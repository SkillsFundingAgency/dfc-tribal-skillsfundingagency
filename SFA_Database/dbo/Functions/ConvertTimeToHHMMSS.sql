--
-- Converts a numeric time period into HH:MM:SS formatted time period
-- Cribbed from StackOverflow:
-- http://stackoverflow.com/questions/1262497/how-to-convert-seconds-to-hhmmss-using-t-sql/11191244#11191244
--
-- Usage:
-- select dbo.ConvertTimeToHHMMSS(123, 's')
-- select dbo.ConvertTimeToHHMMSS(96.999, 'mi')
-- select dbo.ConvertTimeToHHMMSS(35791394.999, 'hh')
-- 0:02:03.000
-- 1:36:59.940
-- 35791394:59:56.400
--
CREATE FUNCTION [dbo].[ConvertTimeToHHMMSS]
(
    @time DECIMAL(28,3), 
    @unit VARCHAR(20)
)
RETURNS VARCHAR(20)
AS
BEGIN

    DECLARE @seconds DECIMAL(18,3), @minutes INT, @hours INT;

    IF(@unit = 'hour' OR @unit = 'hh' )
        SET @seconds = @time * 60 * 60;
    ELSE IF(@unit = 'minute' OR @unit = 'mi' OR @unit = 'n')
        SET @seconds = @time * 60;
    ELSE IF(@unit = 'second' OR @unit = 'ss' OR @unit = 's')
        SET @seconds = @time;
    ELSE SET @seconds = 0; -- unknown time units

    SET @hours = CONVERT(INT, @seconds /60 / 60);
    SET @minutes = CONVERT(INT, (@seconds / 60) - (@hours * 60 ));
    SET @seconds = @seconds % 60;

    RETURN 
        CONVERT(VARCHAR(9), convert(INT, @hours)) + ':' +
        RIGHT('00' + CONVERT(VARCHAR(2), CONVERT(INT, @minutes)), 2) + ':' +
        RIGHT('00' + CONVERT(VARCHAR(6), @seconds), 6)

END
GO
