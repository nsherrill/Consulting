
select sum(iserverguestcount)
from journal
where istatus_pullback=0
and bforcedclose is null