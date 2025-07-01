"""change_steps_to_array_type

Revision ID: 0e567af37510
Revises: b50b3c78dcb3
Create Date: 2025-05-27 20:14:29.689443

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '0e567af37510'
down_revision: Union[str, None] = 'b50b3c78dcb3'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    pass


def downgrade() -> None:
    """Downgrade schema."""
    pass
