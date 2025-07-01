"""rename ingridients to ingredients

Revision ID: rename_ingridients_table
Revises: 76fb32f39d2f
Create Date: 2025-05-27 15:27:41.000000

"""
from typing import Sequence, Union

from alembic import op

# revision identifiers, used by Alembic.
revision: str = 'rename_ingridients_table'
down_revision: Union[str, None] = '76fb32f39d2f'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.rename_table('ingridients', 'ingredients')


def downgrade() -> None:
    """Downgrade schema."""
    op.rename_table('ingredients', 'ingridients')
